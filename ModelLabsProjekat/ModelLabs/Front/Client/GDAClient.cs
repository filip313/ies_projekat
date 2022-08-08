using FTN.Common;
using FTN.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front.Client
{
    public class GDAClient
    {
        private NetworkModelGDAProxy gdaQueryProxy;
        private ModelResourcesDesc modelResourceDesc;

        public GDAClient()
        {
            modelResourceDesc = new ModelResourcesDesc();
        }

        public NetworkModelGDAProxy GdaQueryPorxy
        {
            get
            {
                if(gdaQueryProxy != null)
                {
                    gdaQueryProxy.Abort();
                    gdaQueryProxy = null;
                }

                gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                gdaQueryProxy.Open();
                return gdaQueryProxy;
            }
        }

        public ModelCode ModelCodeFromDMSType(DMSType type)
        {
            return modelResourceDesc.GetModelCodeFromType(type);
        }

        public List<ModelCode> GetAllConcreteModelCodes()
        {
            try
            {
                return GdaQueryPorxy.GetAllConcreteModels();
            }
            catch (Exception e)
            {
                string message = string.Format("Getting concrete model codes.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return null;
            }
        }

        public List<long> GetAllGIDs()
        {
            try
            {
                return GdaQueryPorxy.GetAllGIDs();
            }
            catch (Exception e)
            {
                string message = string.Format("Getting gids method failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return null;
            }
        }

        public List<ModelCode> GetAllProperties(long gid)
        {
            try
            {
                return GdaQueryPorxy.GetAllProperties(gid);
            }
            catch (Exception e)
            {
                string message = string.Format("Getting properties method failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return null;
            }
        }

        public List<ResourceDescription> GetExtentValues(ModelCode model, List<ModelCode> props)
        {
            int iteratorId;
            int readAtOnce = 2;
            int resourcesLeft = 0;

            List<ResourceDescription> ret = new List<ResourceDescription>();

            try
            {
                iteratorId = GdaQueryPorxy.GetExtentValues(model, props);
                resourcesLeft = GdaQueryPorxy.IteratorResourcesLeft(iteratorId);

                while(resourcesLeft > 0)
                {
                    var rds = GdaQueryPorxy.IteratorNext(readAtOnce, iteratorId);

                    for(int i = 0; i < rds.Count; i++)
                    {
                        ret.Add(rds[i]);
                    }

                    resourcesLeft = GdaQueryPorxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryPorxy.IteratorClose(iteratorId);
            }
            catch(Exception e)
            {
                string message = string.Format("Getting extent values method failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return ret;
        }

        public List<ModelCode> GetModelProperties(ModelCode code)
        {
            try
            {
                return GdaQueryPorxy.GetAllModelProps(code);
            }
            catch (Exception e)
            {
                string message = string.Format("Getting properties method failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return null;
            }
        }

        public List<ModelCode> GetReferenceProps(long gid)
        {
            try
            {
                return GdaQueryPorxy.GetReferenceProps(gid);
            }
            catch (Exception e)
            {
                string message = string.Format("Getting reference props failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return null;
            }
        }

        public List<ModelCode> GetProperties(List<ModelCode> codes)
        {
            try
            {
                return GdaQueryPorxy.GetProperties(codes);
            }
            catch (Exception e)
            {
                string message = string.Format("Getting props failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return null;
            }
        }

        public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
        {
            try
            {
                return GdaQueryPorxy.GetValues(resourceId, propIds);
            }
            catch (Exception e)
            {
                string message = string.Format("Getting values method failed.\n\t{0}", e.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return null;
            }
        }

        public List<ResourceDescription>GetRelatedValues(long source , Association association, List<ModelCode> props)
        {
            List<ResourceDescription> ret = new List<ResourceDescription>();

            string message = "Getting related values method started.";
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            List<long> resultList = new List<long>();
            int numberOfRes = 2;

            try
            {
                int iteratorId = GdaQueryPorxy.GetRelatedValues(source, props, association);
                int resourecesLeft = GdaQueryPorxy.IteratorResourcesLeft(iteratorId);

                while (resourecesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryPorxy.IteratorNext(numberOfRes, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        ret.Add(rds[i]);
                    }

                    resourecesLeft = GdaQueryPorxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryPorxy.IteratorClose(iteratorId);
                message = "Success";
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            }
            catch(Exception e)
            {
                message = string.Format("Getting related values method  failed for sourceGlobalId = {0} and association (propertyId = {1}, type = {2}). Reason: {3}", source, association.PropertyId, association.Type, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return ret;
        }
    }
}
