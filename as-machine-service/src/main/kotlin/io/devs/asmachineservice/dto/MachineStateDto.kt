package io.devs.asmachineservice.dto

data class MachineStateDto(
    var code: String = "unknown",
    var caption: String = "Неизвестное состояние"
)