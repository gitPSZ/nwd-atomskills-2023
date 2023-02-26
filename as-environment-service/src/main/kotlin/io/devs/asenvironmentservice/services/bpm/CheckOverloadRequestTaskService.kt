package io.devs.asenvironmentservice.services.bpm

import com.tej.JooQDemo.jooq.sample.model.crm_request.Tables
import io.devs.asenvironmentservice.services.EnvironmentValueService
import org.camunda.bpm.engine.delegate.DelegateExecution
import org.joda.time.DateTime
import org.jooq.DSLContext
import org.springframework.stereotype.Service

@Service
class CheckOverloadRequestTaskService(
    private val dsl: DSLContext,
    private val environmentValueService: EnvironmentValueService
) {

    fun check(execution: DelegateExecution) {
        val inProgressRequestCount: Long = dsl.selectCount()
            .from(Tables.REQUEST)
            .innerJoin(Tables.REQUEST_STATES).on(Tables.REQUEST_STATES.S_CODE.eq(Tables.REQUEST.S_STATE_CODE))
            .where(Tables.REQUEST.S_STATE_CODE.eq("DRAFT"))
            .fetchOneInto(Long::class.java) ?: -1L;
        if (inProgressRequestCount >= 0 &&
            inProgressRequestCount < environmentValueService.getMaxExecutingRequests()
        ) {
            execution.setVariable("isOverloadRequest", false);
            println("${DateTime.now()} - Заявок: $inProgressRequestCount. Макс. кол-во заявок: ${environmentValueService.getMaxExecutingRequests()}");
        } else {
            execution.setVariable("isOverloadRequest", true);
        }
    }
}