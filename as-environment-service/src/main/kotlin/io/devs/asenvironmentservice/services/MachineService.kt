package io.devs.asenvironmentservice.services

import com.fasterxml.jackson.databind.JsonNode
import com.fasterxml.jackson.databind.ObjectMapper
import com.tej.JooQDemo.jooq.sample.model.mnf_machines.Routines
import com.tej.JooQDemo.jooq.sample.model.mnf_machines.Tables.MACHINE_LOG
import com.tej.JooQDemo.jooq.sample.model.mnf_machines.Tables.MACHINE_STATES
import io.devs.asenvironmentservice.dto.MachineLogDto
import io.devs.asenvironmentservice.dto.MachineStateDto
import io.devs.asenvironmentservice.dto.ProductDto
import io.devs.asenvironmentservice.enums.MachineStates
import org.camunda.bpm.engine.RuntimeService
import org.jooq.DSLContext
import org.jooq.JSONB
import org.jooq.impl.DSL.coalesce
import org.jooq.impl.DSL.inline
import org.springframework.stereotype.Service
import java.time.LocalDateTime

@Service
class MachineService(
    private val dsl: DSLContext,
    private val runtimeService: RuntimeService,
    private val environmentValueService: EnvironmentValueService,
    private val dictionaryService: DictionaryService
) {

    fun getAllStatuses(machineCode: String): List<MachineLogDto> {
        return dsl
            .select(
                MACHINE_LOG.ID,
                MACHINE_LOG.S_MACHINE_CODE,
                MACHINE_STATES.S_CODE,
                MACHINE_STATES.S_CAPTION,
                MACHINE_LOG.JS_ADV_INFO,
                MACHINE_LOG.D_BEGIN_DATETIME,
                MACHINE_LOG.D_END_DATETIME
            )
            .from(MACHINE_LOG)
            .innerJoin(MACHINE_STATES).on(MACHINE_STATES.S_CODE.eq(MACHINE_LOG.S_STATE_CODE))
            .where(MACHINE_LOG.S_MACHINE_CODE.eq(machineCode))
            .orderBy(MACHINE_LOG.D_BEGIN_DATETIME.asc())
            .fetch { record ->
                MachineLogDto(
                    id = record.get(MACHINE_LOG.ID),
                    code = record.get(MACHINE_LOG.S_MACHINE_CODE),
                    state = MachineStateDto(
                        code = record.get(MACHINE_STATES.S_CODE),
                        caption = record.get(MACHINE_STATES.S_CAPTION)
                    ),
                    advInfo = ObjectMapper().readTree(record.get(MACHINE_LOG.JS_ADV_INFO).data()),
                    beginDateTime = record.get(MACHINE_LOG.D_BEGIN_DATETIME),
                    endDateTime = record.get(MACHINE_LOG.D_END_DATETIME)
                );
            };
    }

    fun getStatus(machineCode: String): MachineLogDto {
        return dsl
            .select(
                MACHINE_LOG.ID,
                MACHINE_LOG.S_MACHINE_CODE,
                MACHINE_STATES.S_CODE,
                MACHINE_STATES.S_CAPTION,
                MACHINE_LOG.JS_ADV_INFO,
                MACHINE_LOG.D_BEGIN_DATETIME,
                MACHINE_LOG.D_END_DATETIME
            )
            .from(MACHINE_LOG)
            .innerJoin(MACHINE_STATES).on(MACHINE_STATES.S_CODE.eq(MACHINE_LOG.S_STATE_CODE))
            .where(MACHINE_LOG.S_MACHINE_CODE.eq(machineCode))
            .and(
                inline(LocalDateTime.now()).between(
                    MACHINE_LOG.D_BEGIN_DATETIME,
                    coalesce(MACHINE_LOG.D_END_DATETIME, inline(LocalDateTime.now()))
                )
            )
            .fetchOne { record ->
                MachineLogDto(
                    id = record.get(MACHINE_LOG.ID),
                    code = record.get(MACHINE_LOG.S_MACHINE_CODE),
                    state = MachineStateDto(
                        code = record.get(MACHINE_STATES.S_CODE),
                        caption = record.get(MACHINE_STATES.S_CAPTION)
                    ),
                    advInfo = ObjectMapper().readTree(record.get(MACHINE_LOG.JS_ADV_INFO).data()),
                    beginDateTime = record.get(MACHINE_LOG.D_BEGIN_DATETIME),
                    endDateTime = record.get(MACHINE_LOG.D_END_DATETIME)
                );
            } ?: MachineLogDto();
    }

    fun setMachineStatus(
        machineCode: String,
        statusCode: MachineStates,
        advInfo: JSONB,
        machineType: String = "unknown"
    ) {
        Routines.machineLog_ChangeState(dsl.configuration(), machineCode, statusCode.code, advInfo);
        if (statusCode == MachineStates.WORKING) {
            val productIdNode: JsonNode? = ObjectMapper().readTree(advInfo.data()).get("productId");
            if (productIdNode != null) {
                val product: ProductDto = dictionaryService.getProduct(productIdNode.asLong());
                val workingDuration: Long = when (machineType) {
                    "lathe" -> product.latheTime
                    "milling" -> product.millingTime
                    else -> -1L
                }
                if (workingDuration > 0L) {
                    val variables: MutableMap<String, Any> =
                        hashMapOf(
                            Pair("workingDuration", "PT${workingDuration}S"),
                            Pair("productId", product.id)
                        )
                    runtimeService.startProcessInstanceByKey("mnf_working_process", machineCode, variables);
                }
            }
        }
        if (statusCode == MachineStates.REPAIRING) {
            val reparingDuration: Long =
                (environmentValueService.getMinRepairingDuration()..environmentValueService.getMaxRepairingDuration()).random();
            val variables: MutableMap<String, Any> = hashMapOf(Pair("reparingDuration", "PT${reparingDuration}S"))
            runtimeService.startProcessInstanceByKey("mnf_repairing_process", machineCode, variables);
        }
    }
}