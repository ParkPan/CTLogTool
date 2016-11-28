/*
 * Created by SharpDevelop.
 * User: z002sajh
 * Date: 11/19/2012
 * Time: 10:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace LogObjects
{
	/// <summary>
	/// Description of LogObjects.
	/// </summary>

	public abstract class ObjectInfo : IComparable<ObjectInfo>
	{
		public string beginTime;
		public string endTime;
		private long duration = 0;
		public DateTime Time
		{
			get
			{
				return DateTime.Parse(beginTime.Substring(0, beginTime.LastIndexOf(":")));
			}
		}
		public long Duration
		{
			get
			{
				if(duration == 0)
				{
					DateTime tempEnd = DateTime.Parse(endTime.Substring(0, endTime.LastIndexOf(":")));
					DateTime tempBegin = DateTime.Parse(beginTime.Substring(0, beginTime.LastIndexOf(":")));
					long tmpEnd = tempEnd.Ticks/10000 + Int64.Parse
						(endTime.Substring(endTime.LastIndexOf(":")+1, endTime.Length-endTime.LastIndexOf(":")-1));
					long tmpBegin = tempBegin.Ticks/10000 + Int64.Parse
						(beginTime.Substring(beginTime.LastIndexOf(":")+1, beginTime.Length-beginTime.LastIndexOf(":")-1));
					duration = tmpEnd - tmpBegin;
				}
				return duration;
			}
		}
		public int CompareTo(ObjectInfo other)
		{
			return this.Time.CompareTo(other.Time);
		}
	}
	
	public class LoadInfo : ObjectInfo
	{
		public string LoadProtocol
		{
			get;
			set;
		}
		public string LoadScanType
		{
			get;
			set;
		}
	}
	
	public class ScanInfo : ObjectInfo
	{
		public string ScanMode
		{
			get;
			set;
		}
		public bool ScanStatus
		{
			get;
			set;
		}
		public ScanRTDReconInfo ScanReconInfo;
		public ScanXrayInfo ScanXray;
		public long ScanDelay
		{
			get
			{
				if(ScanXray == null)
					return 0L;
				DateTime tempEnd = DateTime.Parse(ScanXray.beginTime.Substring(0, ScanXray.beginTime.LastIndexOf(":")));
				DateTime tempBegin = DateTime.Parse(beginTime.Substring(0, beginTime.LastIndexOf(":")));
				long tmpEnd = tempEnd.Ticks/10000 + Int64.Parse
					(ScanXray.beginTime.Substring(ScanXray.beginTime.LastIndexOf(":")+1, ScanXray.beginTime.Length-ScanXray.beginTime.LastIndexOf(":")-1));
				long tmpBegin = tempBegin.Ticks/10000 + Int64.Parse
					(beginTime.Substring(beginTime.LastIndexOf(":")+1, beginTime.Length-beginTime.LastIndexOf(":")-1));
				return tmpEnd - tmpBegin;
			}
		}
	}
	
	public class ScanRTDReconInfo : ObjectInfo
	{
		 
	}
	
	public class ScanXrayInfo : ObjectInfo
	{
		
	}
	
	public class ReconInfo : ObjectInfo
	{
		public bool ReconStatus
		{
			get;
			set;
		}
		public int ImageNumber
		{
			get;
			set;
		}
		public string ReconJobId
		{
			get;
			set;
		}
	}
	
	public class AdditionalInfo : ObjectInfo
	{
		public new string endTime = null;
		public new long Duration
		{
			get
			{
				return 0L;
			}
		}
	}
	
	public class PatientInfo
	{
		public string PatientId;
		public List<StudyInfo> studyList = new List<StudyInfo>();
	}
	
	public class StudyInfo
	{
		public string StudyId;
		public List<LoadInfo> loadList = new List<LoadInfo>();
		public List<ScanInfo> scanList = new List<ScanInfo>();
		public List<ReconInfo> reconList = new List<ReconInfo>();
	}

	public interface IGetObjectInfos
	{
		 bool GetAllObjectsByPatient(out List<PatientInfo> patients, out List<AdditionalInfo> firstImageList,StringBuilder sb);
	}
}