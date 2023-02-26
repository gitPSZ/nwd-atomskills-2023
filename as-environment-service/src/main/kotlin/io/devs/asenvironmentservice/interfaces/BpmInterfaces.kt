package io.devs.asenvironmentservice.interfaces

import org.camunda.bpm.engine.delegate.DelegateExecution
import org.camunda.bpm.engine.delegate.DelegateTask

// EXECUTION
interface BpmExecutionStart {
    fun start(execution: DelegateExecution);
}

interface BpmExecutionEnd {
    fun end(execution: DelegateExecution);
}

interface BpmExecutionExec {
    fun exec(execution: DelegateExecution);
}

// TASK
interface BpmTaskCreate {
    fun create(task: DelegateTask);
}

interface BpmTaskComplete {
    fun complete(task: DelegateTask);
}

interface BpmTaskTimeout {
    fun timeout(task: DelegateTask);
}