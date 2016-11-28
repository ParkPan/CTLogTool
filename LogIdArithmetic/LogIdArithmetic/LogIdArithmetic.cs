/*
 * Created by SharpDevelop.
 * User: z002sajh
 * Date: 11/20/2012
 * Time: 2:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using LogObjects;

namespace LogIdArithmetic
{
	/// <summary>
	/// Description of LogIdArithmetic.
	/// </summary>
	public class LogIdArithmetic : IGetObjectInfos
	{
		private string patternFilePath;
		private string patientIdPattern;
		private string loadBeginPattern;
		private string loadEndPattern;
		private string loadInfoPattern;
		private string scanBeginPattern;
		private string scanEndPattern;
		private string scanRTDBeginPattern;
		private string scanRTDEndPattern;
		private string reconBeginPattern;
		private string reconEndPattern;
		private string firstImagePattern;
		private string scanXrayOnPattern;
		private string scanXrayOffPattern;
		
		public LogIdArithmetic(string filePath)
		{
			this.patternFilePath = filePath;
			if(!GetPatterns())
				throw new Exception("Operating pattern file error.");
		}
		
		private bool GetPatterns()
		{
			bool retValue = false;
			if(!File.Exists(patternFilePath))
				return retValue;
			try
			{
				using(StreamReader sr = new StreamReader(patternFilePath))
				{
					string tempStr;
					while((tempStr=sr.ReadLine()) != null)
					{
						if (tempStr.Contains("[Patient Id]="))
						{
							patientIdPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Load Begin]="))
						{
							loadBeginPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Load End]="))
						{
							loadEndPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Load Info]="))
						{
							loadInfoPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Scan Begin]="))
						{
							scanBeginPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Scan End]="))
						{
							scanEndPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Scan RTD Begin]="))
						{
							scanRTDBeginPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Scan RTD End]="))
						{
							scanRTDEndPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Scan Xray-ON]="))
						{
							scanXrayOnPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Scan Xray-OFF]="))
						{
							scanXrayOffPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Recon Begin]="))
						{
							reconBeginPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[Recon End]="))
						{
							reconEndPattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
						else if(tempStr.Contains("[First Image]="))
						{
							firstImagePattern = tempStr.Split(new string[]{"=>"}, StringSplitOptions.RemoveEmptyEntries)[1];
						}
					}
					sr.Dispose();
					sr.Close();
				}
				if(loadBeginPattern.Equals("") || loadBeginPattern == null ||
				   loadEndPattern.Equals("") || loadEndPattern == null ||
				   loadInfoPattern.Equals("") || loadInfoPattern == null ||
				   scanBeginPattern.Equals("") || scanBeginPattern == null ||
				   scanEndPattern.Equals("") || scanEndPattern == null ||
				   scanRTDBeginPattern.Equals("") || scanRTDBeginPattern == null ||
				   scanRTDEndPattern.Equals("") || scanRTDEndPattern == null ||
				   scanXrayOnPattern.Equals("") || scanXrayOnPattern == null ||
				   scanXrayOffPattern.Equals("") || scanXrayOffPattern == null ||
				   reconBeginPattern.Equals("") || reconBeginPattern == null ||
				   reconEndPattern.Equals("") || reconEndPattern == null ||
				   firstImagePattern.Equals("") || firstImagePattern == null ||
				   patientIdPattern.Equals("") || patientIdPattern == null)
					;
				else
					retValue = true;
			}
			catch
			{ 
			}
			return retValue;
		}
		
		private List<PatientInfo> GetPatientsById(StringBuilder sb)
		{
			bool bFindId, bGetStudyId;
			List<PatientInfo> patientList;
			try
			{
				using(StringReader sr = new StringReader(sb.ToString()))
				{
					patientList = new List<PatientInfo>();
					string tempStr;
					while((tempStr=sr.ReadLine()) != null)
					{
						bFindId = false;
						Match m = Regex.Match(tempStr, patientIdPattern);
						if(m.Success)
						{
							
							foreach(PatientInfo tmpPatient in patientList)
							{
								bGetStudyId = false;
								if(tmpPatient.PatientId.Equals(m.Groups["pid"].Value))
								{
									bFindId = true;
									foreach(StudyInfo tmpStudy in tmpPatient.studyList)
									{
										if(tmpStudy.StudyId.Equals(m.Groups["sid"].Value))
										{
											bGetStudyId = true;
											break;
										}
									}
									if(!bGetStudyId)
									{
										StudyInfo tempStudy = new StudyInfo();
										tempStudy.StudyId = m.Groups["sid"].Value;
										tmpPatient.studyList.Add(tempStudy);
									}
									break;
								}
							}
							if(!bFindId)
							{
								PatientInfo tempPatient = new PatientInfo();
								tempPatient.PatientId = m.Groups["pid"].Value;
								StudyInfo tempStudy = new StudyInfo();
								tempStudy.StudyId = m.Groups["sid"].Value;
								tempPatient.studyList.Add(tempStudy);
								patientList.Add(tempPatient);
							}
						}
					}
					if(sr != null)
					{
						sr.Dispose();
						sr.Close();
					}
				}
			}
			catch
			{
				patientList = null;
			}
			return patientList;
		}
		
		private void FixSequenceScanTime(ref List<PatientInfo> patientsList)
		{
			int tempIndex = -1;
			if(patientsList == null || patientsList.Count == 0)
				return;
			for(int i=0; i<patientsList.Count; i++)
			{
				for(int k=0; k<patientsList[i].studyList.Count; k++)
				{
					for(int j=0; j<patientsList[i].studyList[k].scanList.Count; j++)
					{
						if(patientsList[i].studyList[k].scanList[j].ScanMode.Contains("Sequence"))
						{
							if(patientsList[i].studyList[k].scanList[j].endTime == null)
							{
								if(tempIndex == -1)
									tempIndex = j;
								else
								{
									patientsList[i].studyList[k].scanList.RemoveAt(j);
									j--;
								}
							}
							else if(patientsList[i].studyList[k].scanList[j].endTime != null)
							{
								if(tempIndex != -1)
								{
									patientsList[i].studyList[k].scanList[j].beginTime = patientsList[i].studyList[k].scanList[tempIndex].beginTime;
									patientsList[i].studyList[k].scanList[j].ScanXray = patientsList[i].studyList[k].scanList[tempIndex].ScanXray;
									patientsList[i].studyList[k].scanList.RemoveAt(tempIndex);
									tempIndex = -1;
									j--;
								}
							}
						}
					}
				}
			}
		}
		
		public bool GetAllObjectsByPatient(out List<PatientInfo> patients, out List<AdditionalInfo> firstImageList,StringBuilder sb)
		{
			bool retValue = false;
			Match match;
			string tempStr;
			int i = 0, j = 0;
			patients = new List<PatientInfo>();
			patients = GetPatientsById(sb);
			bool bFindRecon;
			if(patients.Count == 0)
			{
				firstImageList = null;
				return retValue;
			}
			firstImageList = new List<AdditionalInfo>();
			try
			{
				for(i=0; i < patients.Count; i++)
				{
					using(StringReader sr = new StringReader(sb.ToString()))
					{
						List<ScanRTDReconInfo> scanRTDList = new List<ScanRTDReconInfo>();
						List<ScanXrayInfo> scanXrayList = new List<ScanXrayInfo>();
						while((tempStr=sr.ReadLine()) != null)
						{
							if(tempStr.Contains("@PatientLOID@ = "+patients[i].PatientId) || tempStr.Contains("@Patient LOID@="+patients[i].PatientId) || tempStr.Contains("@PatientID@="+patients[i].PatientId))
							{
								for(j=0; j < patients[i].studyList.Count; j++)
								{
									if(tempStr.Contains("@StudyLOID@ = "+patients[i].studyList[j].StudyId) || tempStr.Contains("@StudyID@="+patients[i].studyList[j].StudyId) || tempStr.Contains("@Study LOID@="+patients[i].studyList[j].StudyId))
										break;
								}
								if(j >= patients[i].studyList.Count)
									continue;
								match = Regex.Match(tempStr, loadBeginPattern);
								if(match.Success)
								{
									LoadInfo tmpLoad = new LoadInfo();
									tmpLoad.beginTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
									patients[i].studyList[j].loadList.Add(tmpLoad);
									continue;
								}
								match = Regex.Match(tempStr, loadEndPattern);
								if(match.Success)
								{
									if(patients[i].studyList[j].loadList.Count != 0)
									{
										patients[i].studyList[j].loadList[patients[i].studyList[j].loadList.Count-1].endTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
									}
									continue;
								}
								match = Regex.Match(tempStr, loadInfoPattern);
								if(match.Success)
								{
									if(patients[i].studyList[j].loadList.Count != 0)
									{
										patients[i].studyList[j].loadList[patients[i].studyList[j].loadList.Count-1].LoadProtocol = match.Groups["protocol"].Value;
										patients[i].studyList[j].loadList[patients[i].studyList[j].loadList.Count-1].LoadScanType = match.Groups["protype"].Value;
									}
									continue;
								}
								match = Regex.Match(tempStr, scanBeginPattern);
								if(match.Success)
								{
									ScanInfo tmpScan = new ScanInfo();
									tmpScan.ScanMode = match.Groups["scantype"].Value;
									tmpScan.beginTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
									patients[i].studyList[j].scanList.Add(tmpScan);
									continue;
								}
								match = Regex.Match(tempStr, scanEndPattern);
								if(match.Success)
								{
									if(patients[i].studyList[j].scanList.Count != 0)
									{
										patients[i].studyList[j].scanList[patients[i].studyList[j].scanList.Count-1].ScanStatus = match.Groups["scanstatus"].Value.ToLower().Equals("success");
										patients[i].studyList[j].scanList[patients[i].studyList[j].scanList.Count-1].endTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
									}
									continue;
								}
								match = Regex.Match(tempStr, scanRTDBeginPattern);
								if(match.Success)
								{
									ScanRTDReconInfo tmpScanRTD = new ScanRTDReconInfo();
									tmpScanRTD.beginTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
									scanRTDList.Add(tmpScanRTD);
									continue;
								}
								match = Regex.Match(tempStr, scanRTDEndPattern);
								if(match.Success)
								{
									if(scanRTDList.Count != 0)
									{
										scanRTDList[scanRTDList.Count-1].endTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
										if(patients[i].studyList[j].scanList.Count != 0)
										{
											patients[i].studyList[j].scanList[patients[i].studyList[j].scanList.Count-1].ScanReconInfo = scanRTDList[scanRTDList.Count-1];
										}
									}
									continue;
								}
								match = Regex.Match(tempStr, scanXrayOnPattern);
								if(match.Success)
								{
									ScanXrayInfo tmpScanXray = new ScanXrayInfo();
									tmpScanXray.beginTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
									scanXrayList.Add(tmpScanXray);
									continue;
								}
								match = Regex.Match(tempStr, scanXrayOffPattern);
								if(match.Success)
								{
									if(scanXrayList.Count != 0)
									{
										scanXrayList[scanXrayList.Count-1].endTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
										if(patients[i].studyList[j].scanList.Count != 0)
										{
											patients[i].studyList[j].scanList[patients[i].studyList[j].scanList.Count-1].ScanXray = scanXrayList[scanXrayList.Count-1];
										}
									}
									continue;
								}
								match = Regex.Match(tempStr, reconBeginPattern);
								if(match.Success)
								{
									bFindRecon = false;
									for(int k=0; k<patients[i].studyList[j].reconList.Count; k++)
									{
										if(patients[i].studyList[j].reconList[k].ReconJobId.Equals(match.Groups["rid"].Value))
										{
											patients[i].studyList[j].reconList[k].ImageNumber += Int32.Parse(match.Groups["imgNO"].Value);
											bFindRecon = true;
											break;
										}
									}
									if(bFindRecon)
										continue;
									ReconInfo tmpRecon = new ReconInfo();
									tmpRecon.ReconJobId = match.Groups["rid"].Value;
									tmpRecon.ImageNumber = Int32.Parse(match.Groups["imgNO"].Value);
									tmpRecon.beginTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
									patients[i].studyList[j].reconList.Add(tmpRecon);
									continue;
								}
								match = Regex.Match(tempStr, reconEndPattern);
								if(match.Success)
								{
									if(patients[i].studyList[j].reconList.Count != 0)
									{
										if(patients[i].studyList[j].reconList[patients[i].studyList[j].reconList.Count-1].ReconJobId.Equals(match.Groups["rid"].Value))
										{
											patients[i].studyList[j].reconList[patients[i].studyList[j].reconList.Count-1].ReconStatus = match.Groups["reconstatus"].Value.ToLower().Equals("success");
											patients[i].studyList[j].reconList[patients[i].studyList[j].reconList.Count-1].endTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
										}
									}
									continue;
								}
								continue;
								//
							}
							match = Regex.Match(tempStr, firstImagePattern);
							if(match.Success && i == 0)
							{
								AdditionalInfo tmpAdd = new AdditionalInfo();
								tmpAdd.beginTime = (match.Groups["time"].Value.Split(':').Length>3 ? match.Groups["time"].Value : match.Groups["time"].Value+":001");
								firstImageList.Add(tmpAdd);
							}
						}
						if(sr != null)
						{
							sr.Dispose();
							sr.Close();
						}
					}
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			if(i >= patients.Count)
			{
				FixSequenceScanTime(ref patients);
				retValue = true;
			}
			return retValue;
		}
	
//		public static void Main(string[] args)
//		{
//			LogIdArithmetic aa = new LogIdArithmetic(@"C:\Documents and Settings\z002sajh\Desktop\LogAnalyse\PatternCustomer.txt");
//			IGetObjectInfos bb = aa as IGetObjectInfos;
//			if(bb != null)
//			{
//				StringBuilder sb = new StringBuilder();
//				using(StreamReader sr = new StreamReader(@"C:\Documents and Settings\z002sajh\Desktop\LogAnalyse\EvtApplication_20121205.txt"))
//				{
//					string tt;
//					while((tt=sr.ReadLine()) != null)
//					{
//						sb.Insert(0, tt+"\n");
//					}
//				}
//				List<PatientInfo> patients;
//				List<AdditionalInfo> firstImageList;				
//				bb.GetAllObjectsByPatient(out patients, out firstImageList, sb);
//				Console.WriteLine("ps: " + patients.Count);
//				Console.WriteLine("img: " + firstImageList.Count);
//				Console.ReadLine();
//			}
//		}		
	}
}