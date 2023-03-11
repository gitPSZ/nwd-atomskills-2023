package io.devs.asenvironmentservice.dto

data class MachineStateDto(
    var code: String = "unknown",
    var caption: String = "Неизвестное состояние"
)