package io.devs.asenvironmentservice.services.bpm

import io.devs.asenvironmentservice.enums.MachineStates
import io.devs.asenvironmentservice.interfaces.BpmExecutionEnd
import io.devs.asenvironmentservice.services.MachineService
import org.camunda.bpm.engine.delegate.DelegateExecution
import org.jooq.JSONB
import org.springframework.stereotype.Service

@Service
class MnfEndReparingEventService(private val machineService: MachineService) : BpmExecutionEnd {
    override fun end(execution: DelegateExecution) {
        val machineCode: String = execution.businessKey;
        machineService.setMachineStatus(
            machineCode,
            MachineStates.WAITING,
            JSONB.valueOf("{\"status\":\"Станок $machineCode починен.\"}")
        );
    }
}