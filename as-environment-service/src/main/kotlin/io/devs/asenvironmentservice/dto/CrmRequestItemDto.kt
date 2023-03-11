package io.devs.asenvironmentservice.dto

data class CrmRequestItemDto(
    var id: Long = 0L,
    var request: CrmRequestDto = CrmRequestDto(),
    var product: ProductDto = ProductDto(),
    var quantity: Long = 0L,
    var quantityExec: Long = 0L,
    )