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
        private BulldozeManager _bulldozeManager;
        private SimulationManager _simulationManager;
        private BuildingManager _buildingManager;
        private Building _building;
        private List<ushort> _buildingIds;
        private bool _running;
        private int _cachedInterval;
        private float _timer;
        private bool _intervalPassed;

        public override void OnCreated(IThreading threading)
        {
            try
            {
                _modConfig = ModConfig.Instance;
                _statistics = Statistics.Instance;
                _bulldozeManager = BulldozeManager.Instance;
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

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
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
                        case 4:
                            _timer += realTimeDelta;
                            if (_timer > 5f)
                            {
                                _timer = _timer - 5f;
                                _intervalPassed = true;
                            }
                            break;
                        case 5:
                            _timer += realTimeDelta;
                            if (_timer > 10f)
                            {
                                _timer = _timer - 10f;
                                _intervalPassed = true;
                            }
                            break;
                        case 6:
                            _timer += realTimeDelta;
                            if (_timer > 30f)
                            {
                                _timer = _timer - 30f;
                                _intervalPassed = true;
                            }
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
                            else if ((_building.m_flags & Building.Flags.BurnedDown) != Building.Flags.None || (_building.m_flags & Building.Flags.Collapsed) != Building.Flags.None)
                            {
                                if (IsDisasterServiceRequired(_building))
                                {
                                    continue;
                                }

                                if (_modConfig.BurnedDownBuildings && (_building.m_problems & Notification.Problem.Fire) != Notification.Problem.None)
                                {
                                    _buildingIds.Add(i);
                                    _statistics.BurnedDownBuildingsBulldozed++;
                                }
                                else if (_modConfig.CollapsedBuildings && ((_building.m_problems & Notification.Problem.StructureDamaged) != Notification.Problem.None || (_building.m_problems & Notification.Problem.StructureVisited) != Notification.Problem.None))
                                {
                                    _buildingIds.Add(i);
                                    _statistics.CollapsedBuildingsBulldozed++;
                                }
                            }
                            else if (_modConfig.FloodedBuildings && (_building.m_flags & Building.Flags.Flooded) != Building.Flags.None)
                            {
                                _buildingIds.Add(i);
                                _statistics.FloodedBuildingsBulldozed++;
                            }
                        }

                        if (!_bulldozeManager.IgnoreMaxAtRunOnce && _buildingIds.Count >= _modConfig.MaxBuildingsPerInterval)
                        {
                            break;
                        }
                    }

                    BulldozeUtils.DeleteBuildings(_buildingIds);

                    _bulldozeManager.FinishedRunOnce = true;

                    _running = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Threading:OnUpdate -> Exception: " + e.Message);
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

        private bool IsDisasterServiceRequired(Building building)
        {
            bool isDisasterServiceRequired = false;

            if (!_modConfig.IgnoreSearchingForSurvivors)
            {
                isDisasterServiceRequired = building.m_levelUpProgress != 255 ? true : false;
            }
            else
            {
                isDisasterServiceRequired = false;
            }

            return isDisasterServiceRequired;
        }
    }
}
