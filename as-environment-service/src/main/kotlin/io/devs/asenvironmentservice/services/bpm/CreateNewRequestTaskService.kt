package io.devs.asenvironmentservice.services.bpm

import com.tej.JooQDemo.jooq.sample.model.crm_request.Routines
import io.devs.asenvironmentservice.interfaces.BpmExecutionExec
import org.camunda.bpm.engine.delegate.DelegateExecution
import org.joda.time.DateTime
import org.jooq.DSLContext
import org.springframework.stereotype.Service

@Service
class CreateNewRequestTaskService(private val dsl: DSLContext) : BpmExecutionExec {
    override fun exec(execution: DelegateExecution) {
        val newRequestId: Long = Routines.request_CreateRequest(dsl.configuration());
        println("${DateTime.now()} - Создали новый заказ. ID = $newRequestId");
    }
}