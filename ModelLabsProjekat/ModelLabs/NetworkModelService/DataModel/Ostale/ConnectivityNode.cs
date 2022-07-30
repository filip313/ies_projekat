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


    /// Connectivity nodes are points where terminals of conducting equipment are connected together with zero impedance.
    public class ConnectivityNode : IdentifiedObject {
        
        private string description;
        private List<long> terminals = new List<long>();

        public List<long> Terminals
        {
            get { return terminals; }
            set { terminals = value; }
        }


        public ConnectivityNode(long globalId) : base(globalId) { }

        public string Description {
            get {
                return this.description;
            }
            set {
                this.description = value;
            }
        }
        

        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
                ConnectivityNode obj = (ConnectivityNode)x;
                return this.Description == obj.Description && (CompareHelper.CompareLists(this.terminals, obj.terminals));
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
                case ModelCode.CONNECTNODE_DESC:
                case ModelCode.CONNECTNODE_TERMINALS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONNECTNODE_DESC:
                    property.SetValue(this.Description);
                    break;
                case ModelCode.CONNECTNODE_TERMINALS:
                    property.SetValue(this.terminals);
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
                case ModelCode.CONNECTNODE_DESC:
                    this.Description = property.AsString();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }


        #endregion


        #region IReference Implementation

        public override bool IsReferenced
        {
            get
            {
                return terminals.Count > 0 || base.IsReferenced;
            }

        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (terminals != null && terminals.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.CONNECTNODE_TERMINALS] = terminals.GetRange(0, terminals.Count);
            }
            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.CONNECTNODE_TERMINALS:
                    terminals.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.CONNECTNODE_TERMINALS:
                    if (terminals.Contains(globalId))
                    {
                        terminals.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion
    }
}