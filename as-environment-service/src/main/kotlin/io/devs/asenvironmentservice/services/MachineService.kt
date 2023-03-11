package io.devs.asenvironmentservice.services

import com.tej.JooQDemo.jooq.sample.model.mnf_machines.Routines
import com.tej.JooQDemo.jooq.sample.model.mnf_machines.Tables.MACHINE_LOG
import com.tej.JooQDemo.jooq.sample.model.mnf_machines.Tables.MACHINE_STATES
import io.devs.asenvironmentservice.dto.MachineLogDto
import io.devs.asenvironmentservice.dto.MachineStateDto
import io.devs.asenvironmentservice.enums.MachineStates
import org.jooq.DSLContext
import org.jooq.JSONB
import org.jooq.impl.DSL.coalesce
import org.jooq.impl.DSL.inline
import org.springframework.stereotype.Service
import java.time.LocalDateTime

@Service
class MachineService(private val dsl: DSLContext) {

    fun getStatus(machineCode: String): MachineLogDto {
        val machineLogDto: MachineLogDto = dsl
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
            .innerJoin(MACHINE_STATES).on(MACHINE_LOG.S_MACHINE_CODE.eq(MACHINE_STATES.S_CODE))
            .where(MACHINE_LOG.S_MACHINE_CODE.eq(machineCode))
            .and(
                inline(LocalDateTime.now()).between(
                    MACHINE_LOG.D_BEGIN_DATETIME,
                    coalesce(MACHINE_LOG.D_END_DATETIME, inline(LocalDateTime.now()))
                )
            )
            .fetchOne { record ->
                val dto = MachineLogDto(
                    id = record.get(MACHINE_LOG.ID),
                    code = record.get(MACHINE_LOG.S_MACHINE_CODE),
                    state = MachineStateDto(
                        code = record.get(MACHINE_STATES.S_CODE),
                        caption = record.get(MACHINE_STATES.S_CAPTION)
                    ),
                    advInfo = record.get(MACHINE_LOG.JS_ADV_INFO),
                    beginDateTime = record.get(MACHINE_LOG.D_BEGIN_DATETIME),
                    endDateTime = record.get(MACHINE_LOG.D_END_DATETIME)
                );
                return@fetchOne dto;
            } ?: MachineLogDto();
        return machineLogDto;
    }

    fun setMachineStatus(machineCode: String, statusCode: MachineStates, advInfo: JSONB) {
        Routines.machineLog_ChangeState(dsl.configuration(), machineCode, statusCode.code, advInfo);
    }
}