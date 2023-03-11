package io.devs.asenvironmentservice.enums

enum class CrmRequestStates(val code: String) {
    DRAFT("DRAFT"),
    IN_PRODUCTION("IN_PRODUCTION"),
    EXECUTED("EXECUTED"),
    CLOSED("CLOSED")
}