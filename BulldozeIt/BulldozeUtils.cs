using ColossalFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulldozeIt
{
    public static class BulldozeUtils
    {
        public static void DeleteBuildings(List<ushort> list)
        {
            try
            {
                SimulationManager simulationManager = Singleton<SimulationManager>.instance;

                foreach (ushort buildingId in list)
                {
                    simulationManager.AddAction(DeleteBuilding(buildingId));
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] BulldozeUtils:DeleteBuildings -> Exception: " + e.Message);
            }
        }

        private static IEnumerator DeleteBuilding(ushort buildingId)
        {
            try
            {
                SimulationManager simulationManager = Singleton<SimulationManager>.instance;
                BuildingManager buildingManager = Singleton<BuildingManager>.instance;
                Building building = buildingManager.m_buildings.m_buffer[buildingId];
                BuildingInfo buildingInfo = building.Info;

                if (building.m_flags != 0)
                {
                    if (simulationManager.IsRecentBuildIndex(building.m_buildIndex))
                    {
                        int buildingRefundAmount = buildingInfo.m_buildingAI.GetRefundAmount(buildingId, ref building);
                        Singleton<EconomyManager>.instance.AddResource(EconomyManager.Resource.RefundAmount, buildingRefundAmount, buildingInfo.m_class);
                    }

                    buildingManager.ReleaseBuilding(buildingId);

                    int publicServiceIndex = ItemClass.GetPublicServiceIndex(buildingInfo.m_class.m_service);
                    if (publicServiceIndex != -1)
                    {
                        Singleton<CoverageManager>.instance.CoverageUpdated(buildingInfo.m_class.m_service, buildingInfo.m_class.m_subService, buildingInfo.m_class.m_level);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] BulldozeUtils:DeleteBuilding -> Exception: " + e.Message);
            }

            yield return null;
        }
    }
}
