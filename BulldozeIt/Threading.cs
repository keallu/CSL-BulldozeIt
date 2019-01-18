using ColossalFramework;
using ICities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulldozeIt
{
    public class Threading : ThreadingExtensionBase
    {
        private ModConfig _modConfig;
        private Statistics _statistics;
        private SimulationManager _simulationManager;
        private BuildingManager _buildingManager;
        private Building _building;
        private List<ushort> _buildingIds;
        private bool _running;
        private int _cachedInterval;
        private bool _intervalPassed;

        public override void OnCreated(IThreading threading)
        {
            try
            {
                _modConfig = ModConfig.Instance;
                _statistics = Statistics.Instance;
                _buildingManager = Singleton<BuildingManager>.instance;
                _simulationManager = Singleton<SimulationManager>.instance;
                _buildingIds = new List<ushort>();
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Threading:OnCreated -> Exception: " + e.Message);
            }
        }

        public override void OnReleased()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Threading:OnReleased -> Exception: " + e.Message);
            }
        }

        public override void OnAfterSimulationTick()
        {
            try
            {
                if (!_running)
                {
                    switch (_modConfig.Interval)
                    {
                        case 1:
                            _intervalPassed = _simulationManager.m_currentGameTime.Day != _cachedInterval ? true : false;
                            _cachedInterval = _simulationManager.m_currentGameTime.Day;
                            break;
                        case 2:
                            _intervalPassed = _simulationManager.m_currentGameTime.Month != _cachedInterval ? true : false;
                            _cachedInterval = _simulationManager.m_currentGameTime.Month;
                            break;
                        case 3:
                            _intervalPassed = _simulationManager.m_currentGameTime.Year != _cachedInterval ? true : false;
                            _cachedInterval = _simulationManager.m_currentGameTime.Year;
                            break;
                        default:
                            break;
                    }
                }

                if (_intervalPassed)
                {
                    _running = true;

                    _intervalPassed = false;

                    _buildingIds.Clear();

                    for (ushort i = 0; i < _buildingManager.m_buildings.m_buffer.Length; i++)
                    {
                        _building = _buildingManager.m_buildings.m_buffer[i];

                        if (IsRICOBuilding(_building))
                        {
                            if (_modConfig.PreserveHistoricalBuildings && (_building.m_flags & Building.Flags.Historical) != Building.Flags.None)
                            {
                                continue;
                            }

                            if (_modConfig.AbandonedBuildings && (_building.m_flags & Building.Flags.Abandoned) != Building.Flags.None)
                            {
                                _buildingIds.Add(i);
                                _statistics.AbandonedBuildingsBulldozed++;
                            }
                            else if (_modConfig.BurnedDownBuildings && (_building.m_flags & Building.Flags.BurnedDown) != Building.Flags.None)
                            {
                                _buildingIds.Add(i);
                                _statistics.BurnedDownBuildingsBulldozed++;
                            }
                            else if (_modConfig.CollapsedBuildings && (_building.m_flags & Building.Flags.Collapsed) != Building.Flags.None)
                            {
                                _buildingIds.Add(i);
                                _statistics.CollapsedBuildingsBulldozed++;
                            }
                            else if (_modConfig.FloodedBuildings && (_building.m_flags & Building.Flags.Flooded) != Building.Flags.None)
                            {
                                _buildingIds.Add(i);
                                _statistics.FloodedBuildingsBulldozed++;
                            }
                        }

                        if (_buildingIds.Count >= _modConfig.MaxBuildingsPerInterval)
                        {
                            break;
                        }
                    }

                    BulldozeUtils.DeleteBuildings(_buildingIds);

                    _running = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Threading:OnAfterSimulationTick -> Exception: " + e.Message);
                _running = false;
            }
        }

        private bool IsRICOBuilding(Building building)
        {
            bool isRICO = false;

            switch (building.Info.m_class.GetZone())
            {
                case ItemClass.Zone.ResidentialHigh:
                case ItemClass.Zone.ResidentialLow:
                case ItemClass.Zone.Industrial:
                case ItemClass.Zone.CommercialHigh:
                case ItemClass.Zone.CommercialLow:
                case ItemClass.Zone.Office:
                    isRICO = true;
                    break;
                default:
                    isRICO = false;
                    break;
            }

            return isRICO;
        }
    }
}
