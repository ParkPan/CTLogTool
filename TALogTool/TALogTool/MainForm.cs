/*
 * Created by SharpDevelop.
 * User: z002sajh
 * Date: 11/20/2012
 * Time: 2:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using LogObjects;
using System.Reflection;
//using LogIdArithmetic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace TALogTool
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private string cfgFilePath, logFilePath;
		private string filterLogBegin, filterLogEnd, arithmeticName;
		private bool bIncludeTALog, bPreDealPattern;
		private StringBuilder logSB = new StringBuilder();
		private List<PatientInfo> patientsList = null;
		private List<AdditionalInfo> firstImagesList = null;
		private List<string> patternList = null;
		
		private bool bAnalyzeDone = false;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private void CfgBtnClick(object sender, EventArgs e)
		{
			if(patternList == null)
				patternList = new List<string>();
			else
				patternList.Clear();
			if(cfgFileDialog.ShowDialog() == DialogResult.OK)
			{
				cfgTextBox.Text = cfgFileDialog.FileName;
			}
			else
			{
				cfgTextBox.Text = "";
			}
			cfgFilePath = cfgTextBox.Text.Trim();
			if(cfgFilePath != null && !cfgFilePath.Equals(""))
			{
				using(StreamReader sr = new StreamReader(cfgFilePath))
				{
					string tempStr;
					while((tempStr=sr.ReadLine()) != null)
					{
						if (tempStr.Contains("[Arithmetic Class]="))
						{
							arithmeticName = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else
						{
							if(tempStr.Contains("=>"))
								patternList.Add(tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1]);
						}
					}
					sr.Dispose();
					sr.Close();
				}
			}
			else
			{
				MessageBox.Show("please input config pattern file path");
			}
		}
		
		private void LogBtnClick(object sender, EventArgs e)
		{
			if(logFileDialog.ShowDialog() == DialogResult.OK)
			{
				logTextBox.Text = logFileDialog.FileName;
			}
			else
			{
				logTextBox.Text = "";
			}
			logFilePath = logTextBox.Text.Trim();
			if(logFilePath == null || logFilePath.Equals(""))
				MessageBox.Show("please input log file path");
		}
		
		private void AlyBtnClick(object sender, EventArgs e)
		{
			if(!File.Exists(cfgFilePath))
			{
				MessageBox.Show("config pattern file does not exist. please check...");
				return;
			}
			if(!File.Exists(logFilePath))
			{
				MessageBox.Show("event log file does not exist. please check...");
				return;
			}
			filterLogBegin = beginLogText.Text.Trim();
			filterLogEnd = endLogText.Text.Trim();
			bIncludeTALog = taLogCheckBox.Checked;
			bPreDealPattern = PreDeal.Checked;

			GetLogsByFilter();
			Task analyzeTask = new Task(AnalyzeLogs);
			analyzeTask.Start();
			progressBar1.Value = 0;
			while(!bAnalyzeDone)
			{
				Thread.Sleep(10000);
				progressBar1.Value++;
			}
			bAnalyzeDone = false;
			progressBar1.Value = 0;
			
			//AnalyzeLogs();

		}
		
		private void GetLogsByFilter()
		{
			logSB.Clear();
			Match match;
			List<string> tmpLS = new List<string>();
			if(filterLogBegin != null && !filterLogBegin.Equals("") &&
			   filterLogEnd != null && !filterLogEnd.Equals(""))
			{
				using(StreamReader sr = new StreamReader(logFilePath))
				{
					string tempStr;
					bool isRecord = false;
					while(true)
					{
						tempStr = sr.ReadLine();
						if(tempStr == null || tempStr.Equals(""))
							break;
						if(tempStr.Contains(filterLogBegin))
							isRecord = true;
						if(isRecord)
						{
							if(bIncludeTALog)
							{
								if(bPreDealPattern)
								{
									foreach(string tmpPattern in patternList)
									{
										match = Regex.Match(tempStr, tmpPattern);
										if(match.Success)
										{
											tmpLS.Add(tempStr);
											break;
										}
									}
								}
								else
								{
									tmpLS.Add(tempStr);
								}
							}
							else
							{
								if(!tempStr.Contains("	CT_TA	"))
								{
									if(bPreDealPattern)
									{
										foreach(string tmpPattern in patternList)
										{
											match = Regex.Match(tempStr, tmpPattern);
											if(match.Success)
											{
												tmpLS.Add(tempStr);
												break;
											}
										}
									}
									else
									{
										tmpLS.Add(tempStr);
									}
								}
							}
						}
						if(tempStr.Contains(filterLogEnd))
							isRecord = false;
					}
				}
			}
			else
			{
				using(StreamReader sr = new StreamReader(logFilePath))
				{
					string tempStr;
					while(true)
					{
						tempStr = sr.ReadLine();
						if(tempStr == null || tempStr.Equals(""))
							break;
						if(bIncludeTALog)
						{
							if(bPreDealPattern)
							{
								foreach(string tmpPattern in patternList)
								{
									match = Regex.Match(tempStr, tmpPattern);
									if(match.Success)
									{
										tmpLS.Add(tempStr);
										break;
									}
								}
							}
							else
							{
								tmpLS.Add(tempStr);
							}
						}
						else
						{
							if(!tempStr.Contains("	CT_TA	"))
							{
								if(bPreDealPattern)
								{
									foreach(string tmpPattern in patternList)
									{
										match = Regex.Match(tempStr, tmpPattern);
										if(match.Success)
										{
											tmpLS.Add(tempStr);
											break;
										}
									}
								}
								else
								{
									tmpLS.Add(tempStr);
								}
							}
						}
					}
				}
			}
			int LSCount = tmpLS.Count;
			for (int i = LSCount-1; i >= 0; i--)
			{
				logSB.Append(tmpLS[i] + "\n");
			}
		}
		
		private void AnalyzeLogs()
		{
			Assembly assembly;
			Type type;
			object obj;
			if(logSB.Length == 0)
			{
				bAnalyzeDone = true;
				MessageBox.Show("Filter log content is empty, Analyse exit...");
				return;
			}
			if(patientsList != null)
				patientsList.Clear();
			if(firstImagesList != null)
				firstImagesList.Clear();
			try
			{
				assembly =Assembly.Load(arithmeticName);
				type = assembly.GetType(arithmeticName+"."+arithmeticName);
			}
			catch
			{
				bAnalyzeDone = true;
				MessageBox.Show("Load arithmetic class failed, Analyse exit...");
				return;
			}
			string[] param = new string[] {cfgFilePath};
			try
			{
				obj = Activator.CreateInstance(type, param);
			}
			catch
			{
				bAnalyzeDone = true;
				MessageBox.Show("Create arithmetic class instance failed, Analyse exit...");
				return;
			}
			IGetObjectInfos arithmeticOBJ = obj as IGetObjectInfos;
			if(arithmeticOBJ == null)
			{
				bAnalyzeDone = true;
				MessageBox.Show("Create arithmetic class instance failed, Analyse exit...");
				return;
			}
			if(arithmeticOBJ.GetAllObjectsByPatient(out patientsList, out firstImagesList, logSB))
			{
				try
				{
					OutputResultFile();
					bAnalyzeDone = true;
					MessageBox.Show("Analyse finish, output result file at log file directory.");
				}
				catch
				{
					bAnalyzeDone = true;
					MessageBox.Show("Analyse finish, output result file error, please contact admin to check code...");
				}
			}
			else
			{
				bAnalyzeDone = true;
				MessageBox.Show("Analyse function execute failed. maybe log or pattern file format issue, please check. Analyse exit...");
			}
		}
		
		private void OutputResultFile()
		{
			string[] fileHeader = new string[]{"PId","StudyId","Protocol","Load1_BeginT","Load1_EndT","Load2_BeginT","Load2_EndT","Load3_BeginT","Load3_EndT","Scan1_BeginT","Scan1_EndT","Scan1_Type",
				"Scan2_BeginT","Scan2_EndT","Scan2_Type","Scan3_BeginT","Scan3_EndT","Scan3_Type","Recon1_BeginT","Recon1_EndT","Recon1_1stImg","Recon1_Num","Recon2_BeginT","Recon2_EndT","Recon2_1stImg","Recon2_Num",
				"Recon3_BeginT","Recon3_EndT","Recon3_1stImg","Recon3_Num","Recon4_BeginT","Recon4_EndT","Recon4_1stImg","Recon4_Num","Load1_Duration(s)","Load2_Duration(s)","Load3_Duration(s)","Scan1_Delay(s)","Scan2_Delay(s)",
				"Scan3_Delay(s)","Recon1_Duration(s)","Recon2_Duration(s)","Recon3_Duration(s)","Recon4_Duration(s)","Recon1_1stImgDuration(s)","Recon2_1stImgDuration(s)","Recon3_1stImgDuration(s)","Recon4_1stImgDuration(s)","Recon1_Speed(ima/s)",
				"Recon2_Speed(ima/s)","Recon3_Speed(ima/s)","Recon4_Speed(ima/s)","Recon1_SpeedNo1stImg(ima/s)","Recon2_SpeedNo1stImg(ima/s)","Recon3_SpeedNo1stImg(ima/s)","Recon4_SpeedNo1stImg(ima/s)"};
			string writeLine;
			string resultFile = Path.GetDirectoryName(logFilePath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(logFilePath) + "_Result.csv";
			int tmpCount;
			float tmpImgSpeed;
			if(patientsList == null || patientsList.Count == 0)
				return;
			if(firstImagesList != null)
				firstImagesList.Sort();
			using(StreamWriter sw = new StreamWriter(resultFile))
			{
				writeLine = string.Join(",", fileHeader);
				sw.WriteLine(writeLine);
				foreach(PatientInfo patient in patientsList)
				{
					ClearStringArray(ref fileHeader);
					fileHeader[0] = patient.PatientId;
					foreach(StudyInfo study in patient.studyList)
					{
						tmpCount = 0;
						fileHeader[1] = study.StudyId;
						if(study.loadList.Count > 0)
							fileHeader[2] = study.loadList[0].LoadProtocol + "(" + study.loadList[0].LoadScanType + ")";
						// Because load end pattern is error now, so close output code of load time
//						tmpCount = study.loadList.Count;
//						for(int i=0; i<tmpCount; i++)
//						{
//							fileHeader[i*2+3] = study.loadList[i].beginTime.Split('	')[1];
//							fileHeader[i*2+4] = study.loadList[i].endTime.Split('	')[1];
//							fileHeader[i+34] = Convert.ToString((float)study.loadList[i].Duration/1000.0f);
//							if(i>=2)
//								break;
//						}
						tmpCount = 0;
						tmpCount = study.scanList.Count;
						for(int i=0; i<tmpCount; i++)
						{
							fileHeader[i*3+9] = study.scanList[i].beginTime.Split('	')[1];
							fileHeader[i*3+10] = study.scanList[i].endTime.Split('	')[1];
							fileHeader[i*3+11] = study.scanList[i].ScanMode;
							if(study.scanList[i].ScanXray != null && study.scanList[i].ScanXray.beginTime != null)
								fileHeader[i+37] = Convert.ToString((float)study.scanList[i].ScanDelay/1000.0f);
							if(i>=2)
								break;
						}
						tmpCount = 0;
						tmpCount = study.reconList.Count;
						if(firstImagesList != null)
							firstImagesList.Sort();
						for(int i=0; i<tmpCount; i++)
						{
							fileHeader[i*4+18] = study.reconList[i].beginTime.Split('	')[1];
							fileHeader[i*4+19] = study.reconList[i].endTime.Split('	')[1];
							if(study.reconList[i].ImageNumber != 0)
							{
								if(firstImagesList != null)
								{
									for(int j=0; j<firstImagesList.Count; j++)
									{
										if(firstImagesList[j].CompareTo(study.reconList[i])>=0)
										{
											fileHeader[i*4+20] = firstImagesList[j].beginTime.Split('	')[1];
											DateTime tempEnd = DateTime.Parse(firstImagesList[j].beginTime.Substring(0, firstImagesList[j].beginTime.LastIndexOf(":")));
											DateTime tempBegin = DateTime.Parse(study.reconList[i].beginTime.Substring(0, study.reconList[i].beginTime.LastIndexOf(":")));
											long tmpEnd = tempEnd.Ticks/10000 + Int64.Parse
												(firstImagesList[j].beginTime.Substring(firstImagesList[j].beginTime.LastIndexOf(":")+1,
												                                        firstImagesList[j].beginTime.Length-firstImagesList[j].beginTime.LastIndexOf(":")-1));
											long tmpBegin = tempBegin.Ticks/10000 + Int64.Parse
												(study.reconList[i].beginTime.Substring(study.reconList[i].beginTime.LastIndexOf(":")+1,
												                                        study.reconList[i].beginTime.Length-study.reconList[i].beginTime.LastIndexOf(":")-1));
											fileHeader[i+44] = Convert.ToString((float)(tmpEnd - tmpBegin)/1000.0f);
											tmpImgSpeed = (float)(study.reconList[i].ImageNumber-1)/((float)study.reconList[i].Duration/1000.0f-(float)(tmpEnd - tmpBegin)/1000.0f);
											fileHeader[i+52] = (tmpImgSpeed>=0 ? Convert.ToString(tmpImgSpeed) : "Error");
											firstImagesList.RemoveAt(j);
											break;
										}
										else
										{
											firstImagesList.RemoveAt(j);
											j--;
										}
									}
								}
							}
							fileHeader[i*4+21] = Convert.ToString(study.reconList[i].ImageNumber);
							fileHeader[i+40] = Convert.ToString((float)study.reconList[i].Duration/1000.0f);
							fileHeader[i+48] = Convert.ToString((float)study.reconList[i].ImageNumber/((float)study.reconList[i].Duration/1000.0f));
							if(i>=3)
								break;
						}
						writeLine = string.Join(",", fileHeader);
						sw.WriteLine(writeLine);
						ClearStringArray(ref fileHeader);
					}
				}
			}
		}
		
		private void ClearStringArray(ref string[] cArray)
		{
			for(int i=0; i<cArray.Length; i++)
			{
				cArray[i] = "n.a.";
			}
		}
	}
}