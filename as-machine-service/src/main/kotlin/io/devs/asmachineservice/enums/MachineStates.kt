package io.devs.asmachineservice.enums

enum class MachineStates(val code: String) {
    WAITING("WAITING"),
    WORKING("WORKING"),
    BROKEN("BROKEN")
}