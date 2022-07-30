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


    /// A wire or combination of wires not insulated from one another, with consistent electrical characteristics, used to carry direct current between points in the DC region of the power system.
    public class DCLineSegment : Conductor {

        public DCLineSegment(long globalId) : base(globalId) { }

        public override bool Equals(object x)
        {
            return base.Equals(x);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess Implementation

        public override bool HasProperty(ModelCode property)
        {
            return base.HasProperty(property);
        }

        public override void GetProperty(Property property)
        {
            base.GetProperty(property);
        }

        public override void SetProperty(Property property)
        {
            base.SetProperty(property);
        }
        #endregion

        #region IReference Implementation

        public override bool IsReferenced
        {
            get
            {
                return base.IsReferenced;
            }

        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {

            base.AddReference(referenceId, globalId);

        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            base.RemoveReference(referenceId, globalId);
        }

        #endregion
    }
}
