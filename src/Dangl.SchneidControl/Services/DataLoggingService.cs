﻿using Dangl.SchneidControl.Data;

namespace Dangl.SchneidControl.Services
{
    public class DataLoggingService : IDataLoggingService
    {
        private readonly DataLoggingContext _context;
        private readonly ISchneidReadRepository _schneidReadRepository;

        public DataLoggingService(DataLoggingContext context,
            ISchneidReadRepository schneidReadRepository)
        {
            _context = context;
            _schneidReadRepository = schneidReadRepository;
        }

        public async Task ReadAndSaveValuesAsync()
        {
            try
            {
                var outerTemperature = await _schneidReadRepository.GetOuterTemperaturAsync();
                if (outerTemperature.IsSuccess)
                {
                    _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.OuterTemperature, Value = Convert.ToInt32(outerTemperature.Value.Value * 10) });
                }

                var totalEnergyConsumption = await _schneidReadRepository.GetTotalEnergyConsumptionAsync();
                if (totalEnergyConsumption.IsSuccess)
                {
                    _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.TotalEnergyConsumption, Value = Convert.ToInt32(totalEnergyConsumption.Value.Value * 10) });
                }

                var heatingPowerDraw = await _schneidReadRepository.GetCurrentHeatingPowerDrawAsync();
                if (heatingPowerDraw.IsSuccess)
                {
                    _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.HeatingPowerDraw, Value = Convert.ToInt32(heatingPowerDraw.Value.Value) });
                }

                var bufferTemperature = await _schneidReadRepository.GetBufferTemperatureTopAsync();
                if (bufferTemperature.IsSuccess)
                {
                    _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.BufferTemperature, Value = Convert.ToInt32(bufferTemperature.Value.Value * 10) });
                }

                var boilerTemperature = await _schneidReadRepository.GetBoilerTemperatureTopAsync();
                if (boilerTemperature.IsSuccess)
                {
                    _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.BoilerTemperature, Value = Convert.ToInt32(boilerTemperature.Value.Value * 10) });
                }

                var valveOpening = await _schneidReadRepository.GetValveOpeningAsync();
                if (valveOpening.IsSuccess)
                {
                    _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.ValveOpening, Value = Convert.ToInt32(valveOpening.Value.Value) });
                }

                var heatingCircuitPump0 = await _schneidReadRepository.GetPumpStatusHeatingCircuit00Async();
                if (heatingCircuitPump0.IsSuccess)
                {
                    _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.HeatingCircuit0Pump, Value = heatingCircuitPump0.Value.Value ? 1 : 0 });
                }

                var heatingCircuitPump1 = await _schneidReadRepository.GetPumpStatusHeatingCircuit01Async();
                if (heatingCircuitPump1.IsSuccess)
                {
                    _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.HeatingCircuit0Pump, Value = heatingCircuitPump1.Value.Value ? 1 : 0 });
                }

                await _context.SaveChangesAsync();
            }
            catch
            {
                // We're just ignoring failures here, don't want the task to crash
            }
        }
    }
}
