package io.devs.asenvironmentservice.services.bpm

import io.devs.asenvironmentservice.interfaces.BpmExecutionStart
import org.camunda.bpm.engine.delegate.DelegateExecution
import org.springframework.stereotype.Service

@Service
class CrmAcceptRequestStartEventService : BpmExecutionStart {
    override fun start(execution: DelegateExecution) {
        execution.setVariable("isOverloadRequest", false);
        execution.setVariable("endProcess", false);
    }
}