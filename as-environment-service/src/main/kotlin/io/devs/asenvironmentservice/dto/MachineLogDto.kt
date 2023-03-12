package io.devs.asenvironmentservice.dto

import com.fasterxml.jackson.databind.JsonNode
import com.fasterxml.jackson.databind.ObjectMapper
import java.time.LocalDateTime

data class MachineLogDto(
    var id: Long = 0L,
    var code: String = "unknown",
    var state: MachineStateDto = MachineStateDto(),
    var advInfo: JsonNode = ObjectMapper().readTree("{}"),
    var beginDateTime: LocalDateTime = LocalDateTime.now(),
    var endDateTime: LocalDateTime? = null
)