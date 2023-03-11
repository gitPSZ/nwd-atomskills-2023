package io.devs.asenvironmentservice.dto

data class ProductDto(
    var id: Long = 0L,
    var code: String = "",
    var caption: String = "",
    var millingTime: Long = 0L,
    var latheTime: Long = 0L
)
