namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

		#region Populate ResourceDescription
		public static void PopulateIdentifiedObjectProperties(IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
				}
				if (cimIdentifiedObject.AliasNameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_ALIAS, cimIdentifiedObject.AliasName));
				}
			}
		}


		public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPowerSystemResource != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
			}
		}


		public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);
			}
		}

		public static void PopulateConductingEquipmentProperties(ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimConductingEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);
			}
		}

		public static void PopulateConnectivityNodeProperties(ConnectivityNode cimConnNode, ResourceDescription rd)
		{
			if ((cimConnNode != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimConnNode, rd);
				if (cimConnNode.DescriptionHasValue)
				{
					rd.AddProperty(new Property(ModelCode.CONNECTNODE_DESC, cimConnNode.Description));
				}
			}
		}
		public static void PopulateSeriesCompensator(SeriesCompensator cimSerCom, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimSerCom != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimSerCom, rd, importHelper, report);
				if (cimSerCom.RHasValue)
				{
					rd.AddProperty(new Property(ModelCode.SERIES_COMP_R, cimSerCom.R));
				}
				if (cimSerCom.R0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.SERIES_COMP_R0, cimSerCom.R0));
				}
				if (cimSerCom.XHasValue)
				{
					rd.AddProperty(new Property(ModelCode.SERIES_COMP_X, cimSerCom.X));
				}
				if (cimSerCom.X0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.SERIES_COMP_X0, cimSerCom.X0));
				}
			}
		}

		public static void PopulateConductor(Conductor cimConductor, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
			if((cimConductor != null) && (rd != null))
            {
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimConductor, rd, importHelper, report);
                if (cimConductor.LengthHasValue)
                {
					rd.AddProperty(new Property(ModelCode.COND_LENGTH, cimConductor.Length));
                }
            }
        }

		public static void PopulateDCLineSegment(DCLineSegment cimDCLineSegment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
			if(cimDCLineSegment != null && rd != null)
            {
				PowerTransformerConverter.PopulateConductor(cimDCLineSegment, rd, importHelper, report);
            }
        }

		public static void PopulateACLineSegment(ACLineSegment cimAcLineSegment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimAcLineSegment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductor(cimAcLineSegment, rd, importHelper, report);
				if (cimAcLineSegment.RHasValue)
				{
					rd.AddProperty(new Property(ModelCode.ACLINESEG_R, cimAcLineSegment.R));
				}
				if (cimAcLineSegment.R0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.ACLINESEG_R0, cimAcLineSegment.R0));
				}
				if (cimAcLineSegment.XHasValue)
				{
					rd.AddProperty(new Property(ModelCode.ACLINESEG_X, cimAcLineSegment.X));
				}
				if (cimAcLineSegment.X0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.ACLINESEG_X0, cimAcLineSegment.X0));
				}
				if (cimAcLineSegment.B0chHasValue)
				{
					rd.AddProperty(new Property(ModelCode.ACLINESEG_B0CH, cimAcLineSegment.B0ch));
				}
				if (cimAcLineSegment.R0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.ACLINESEG_BCH, cimAcLineSegment.Bch));
				}
				if (cimAcLineSegment.XHasValue)
				{
					rd.AddProperty(new Property(ModelCode.ACLINESEG_G0CH, cimAcLineSegment.G0ch));
				}
				if (cimAcLineSegment.X0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.ACLINESEG_GCH, cimAcLineSegment.Gch));
				}
                if (cimAcLineSegment.PerLengthImpedanceHasValue)
                {
					long gid = importHelper.GetMappedGID(cimAcLineSegment.PerLengthImpedance.ID);
					if(gid < 0)
                    {
						report.Report.Append("WARNING: Convert ").Append(cimAcLineSegment.GetType().ToString()).Append(" rdfID = \"").Append(cimAcLineSegment.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimAcLineSegment.PerLengthImpedance.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.ACLINESEG_PERLENGTHIMP, gid));
				}
			}

		}

		public static void PopulatePerLengthImpedance(PerLengthImpedance cimPerLengthImp, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPerLengthImp != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPerLengthImp, rd);
			}
		}

		public static void PopulatePerLengthSequenceImpedance(PerLengthSequenceImpedance cimPerLengthSeqImp, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPerLengthSeqImp != null) && (rd != null))
			{
				PowerTransformerConverter.PopulatePerLengthImpedance(cimPerLengthSeqImp, rd, importHelper, report);
				if (cimPerLengthSeqImp.RHasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERLENGTHSEQIMP_R, cimPerLengthSeqImp.R));
				}
				if (cimPerLengthSeqImp.R0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERLENGTHSEQIMP_R0, cimPerLengthSeqImp.R0));
				}
				if (cimPerLengthSeqImp.XHasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERLENGTHSEQIMP_X, cimPerLengthSeqImp.X));
				}
				if (cimPerLengthSeqImp.X0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERLENGTHSEQIMP_X0, cimPerLengthSeqImp.X0));
				}
				if (cimPerLengthSeqImp.B0chHasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERLENGTHSEQIMP_B0CH, cimPerLengthSeqImp.B0ch));
				}
				if (cimPerLengthSeqImp.R0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERLENGTHSEQIMP_BCH, cimPerLengthSeqImp.Bch));
				}
				if (cimPerLengthSeqImp.XHasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERLENGTHSEQIMP_G0CH, cimPerLengthSeqImp.G0ch));
				}
				if (cimPerLengthSeqImp.X0HasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERLENGTHSEQIMP_GCH, cimPerLengthSeqImp.Gch));
				}
			}
		}
		public static void PopulateTerminal(Terminal cimTerminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimTerminal != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimTerminal, rd);

				if (cimTerminal.ConnectivityNodeHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTerminal.ConnectivityNode.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimTerminal.ConnectivityNode.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.TERMINAL_CONNECTNODE, gid));
				}
				if (cimTerminal.ConductingEquipmentHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTerminal.ConductingEquipment.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimTerminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.TERMINAL_CONDEQ, gid));
				}
			}
		}

		#endregion Populate ResourceDescription

		#region Enums convert

		#endregion Enums convert
	}
}
