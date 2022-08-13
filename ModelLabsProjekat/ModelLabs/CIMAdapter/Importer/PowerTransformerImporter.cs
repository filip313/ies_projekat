using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			//// import all concrete model types (DMSType enum)
			ImportConnectivityNodes();
			ImportPerLegnthSequenceImpedances();
			ImportSeriesCompensators();
			ImportDCLineSegments();
			ImportACLineSegments();
			ImportTerminals();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		#region Import
		private void ImportConnectivityNodes()
		{
			SortedDictionary<string, object> cimConnNodes = concreteModel.GetAllObjectsOfType("FTN.ConnectivityNode");
			if (cimConnNodes != null)
			{
				foreach (KeyValuePair<string, object> cimConnNodePair in cimConnNodes)
				{
					ConnectivityNode node = cimConnNodePair.Value as ConnectivityNode;

					ResourceDescription rd = CreateConnNodeResourceDescription(node);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("ConnNode ID = ").Append(node.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("ConnNode ID = ").Append(node.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateConnNodeResourceDescription(ConnectivityNode cimConnNode)
		{
			ResourceDescription rd = null;
			if (cimConnNode != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CONNECTNODE, importHelper.CheckOutIndexForDMSType(DMSType.CONNECTNODE));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimConnNode.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateConnectivityNodeProperties(cimConnNode, rd);
			}
			return rd;
		}

		private void ImportSeriesCompensators()
		{
			SortedDictionary<string, object> cimSeriesComp = concreteModel.GetAllObjectsOfType("FTN.SeriesCompensator");
			if (cimSeriesComp != null)
			{
				foreach (KeyValuePair<string, object> cimSeriesCompPair in cimSeriesComp)
				{
					SeriesCompensator node = cimSeriesCompPair.Value as SeriesCompensator;

					ResourceDescription rd = CreateSeriesCompensatorResourceDescription(node);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("SeriesComp ID = ").Append(node.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("SeriesComp ID = ").Append(node.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateSeriesCompensatorResourceDescription(SeriesCompensator cimSeriesComp)
		{
			ResourceDescription rd = null;
			if (cimSeriesComp != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SERIES_COMP, importHelper.CheckOutIndexForDMSType(DMSType.SERIES_COMP));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimSeriesComp.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateSeriesCompensator(cimSeriesComp, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportDCLineSegments()
		{
			SortedDictionary<string, object> cimSeriesComp = concreteModel.GetAllObjectsOfType("FTN.DCLineSegment");
			if (cimSeriesComp != null)
			{
				foreach (KeyValuePair<string, object> cimSeriesCompPair in cimSeriesComp)
				{
					DCLineSegment node = cimSeriesCompPair.Value as DCLineSegment;

					ResourceDescription rd = CreateDCLineSegmentDescription(node);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("DCLine ID = ").Append(node.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("DCLine ID = ").Append(node.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateDCLineSegmentDescription(DCLineSegment cimSeriesComp)
		{
			ResourceDescription rd = null;
			if (cimSeriesComp != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DCLINESEG, importHelper.CheckOutIndexForDMSType(DMSType.DCLINESEG));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimSeriesComp.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateDCLineSegment(cimSeriesComp, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportACLineSegments()
		{
			SortedDictionary<string, object> cimConnNodes = concreteModel.GetAllObjectsOfType("FTN.ACLineSegment");
			if (cimConnNodes != null)
			{
				foreach (KeyValuePair<string, object> cimConnNodePair in cimConnNodes)
				{
					ACLineSegment node = cimConnNodePair.Value as ACLineSegment;

					ResourceDescription rd = CreateACLineSegmentDescription(node);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("ACLine ID = ").Append(node.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("ACLine ID = ").Append(node.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateACLineSegmentDescription(ACLineSegment cimConnNode)
		{
			ResourceDescription rd = null;
			if (cimConnNode != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ACLINESEG, importHelper.CheckOutIndexForDMSType(DMSType.ACLINESEG));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimConnNode.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateACLineSegment(cimConnNode, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportPerLegnthSequenceImpedances()
		{
			SortedDictionary<string, object> cimConnNodes = concreteModel.GetAllObjectsOfType("FTN.PerLengthSequenceImpedance");
			if (cimConnNodes != null)
			{
				foreach (KeyValuePair<string, object> cimConnNodePair in cimConnNodes)
				{
					PerLengthSequenceImpedance node = cimConnNodePair.Value as PerLengthSequenceImpedance;

					ResourceDescription rd = CreatePerLengthSequenceImpedanceDescription(node);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("PerLen ID = ").Append(node.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("PerLen ID = ").Append(node.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreatePerLengthSequenceImpedanceDescription(PerLengthSequenceImpedance cimConnNode)
		{
			ResourceDescription rd = null;
			if (cimConnNode != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PERLENGTHSEQIMP, importHelper.CheckOutIndexForDMSType(DMSType.PERLENGTHSEQIMP));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimConnNode.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulatePerLengthSequenceImpedance(cimConnNode, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportTerminals()
		{
			SortedDictionary<string, object> cimConnNodes = concreteModel.GetAllObjectsOfType("FTN.Terminal");
			if (cimConnNodes != null)
			{
				foreach (KeyValuePair<string, object> cimConnNodePair in cimConnNodes)
				{
					Terminal node = cimConnNodePair.Value as Terminal;

					ResourceDescription rd = CreateTerminalDescription(node);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Terminal ID = ").Append(node.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Terminal ID = ").Append(node.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateTerminalDescription(Terminal cimConnNode)
		{
			ResourceDescription rd = null;
			if (cimConnNode != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERMINAL, importHelper.CheckOutIndexForDMSType(DMSType.TERMINAL));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimConnNode.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateTerminal(cimConnNode, rd, importHelper, report);
			}
			return rd;
		}
		#endregion Import
	}
}

