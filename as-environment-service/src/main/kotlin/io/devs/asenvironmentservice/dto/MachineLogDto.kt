package io.devs.asenvironmentservice.dto

import org.jooq.JSONB
import java.time.LocalDateTime

data class MachineLogDto(
    var id: Long = 0L,
    var code: String = "unknown",
    var state: MachineStateDto = MachineStateDto(),
    var advInfo: JSONB? = null,
    var beginDateTime: LocalDateTime = LocalDateTime.now(),
    var endDateTime: LocalDateTime? = null
)