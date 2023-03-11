package io.devs.asenvironmentservice.dto


import java.time.LocalDate
import java.time.LocalDateTime

data class CrmRequestDto(
    var id: Long = 0,
    var number: String = "",
    var date: LocalDate = LocalDateTime.now().toLocalDate(),
    var contractor: ContractorDto = ContractorDto(),
    var description: String = "",
    var state: CrmRequestStateDto = CrmRequestStateDto(),
    var releaseDate: LocalDate? = null
)
