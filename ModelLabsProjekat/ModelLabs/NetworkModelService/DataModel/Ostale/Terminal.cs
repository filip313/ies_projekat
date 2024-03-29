//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FTN.Services.NetworkModelService.DataModel.Ostale
{
    using System;
    using System.Collections.Generic;
    using FTN;
    using FTN.Common;
    using FTN.Services.NetworkModelService.DataModel.Core;


    /// An electrical connection point to a piece of conducting equipment. Terminals are connected at physical connection points called connectivity nodes.
    public class Terminal : IdentifiedObject
    {
        public Terminal(long globalId) : base(globalId) { }

        private long connectivityNode;

        public long ConnectivityNode
        {
            get { return connectivityNode; }
            set { connectivityNode = value; }
        }
        private long conductingEquipment;

        public long ConductingEquipment
        {
            get { return conductingEquipment; }
            set { conductingEquipment = value; }
        }

        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
                Terminal obj = (Terminal)x;
                return obj.conductingEquipment == this.conductingEquipment && obj.connectivityNode == this.connectivityNode;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess Implementation

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.TERMINAL_CONDEQ:
                case ModelCode.TERMINAL_CONNECTNODE:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TERMINAL_CONDEQ:
                    property.SetValue(this.conductingEquipment);
                    break;
                case ModelCode.TERMINAL_CONNECTNODE:
                    property.SetValue(this.ConnectivityNode);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TERMINAL_CONDEQ:
                    this.conductingEquipment = property.AsReference();
                    break;
                case ModelCode.TERMINAL_CONNECTNODE:
                    this.connectivityNode = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }


        #endregion

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (connectivityNode != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.TERMINAL_CONNECTNODE] = new List<long> { connectivityNode };
            }

            if (conductingEquipment != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.TERMINAL_CONDEQ] = new List<long> { conductingEquipment };
            }

            base.GetReferences(references, refType);
        }

    }
}
