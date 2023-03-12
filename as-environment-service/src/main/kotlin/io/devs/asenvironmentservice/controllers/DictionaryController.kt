package io.devs.asenvironmentservice.controllers

import io.devs.asenvironmentservice.dto.ContractorDto
import io.devs.asenvironmentservice.dto.ProductDto
import io.devs.asenvironmentservice.services.DictionaryService
import org.springframework.web.bind.annotation.*

@RestController
@CrossOrigin("*")
@RequestMapping("dict")
class DictionaryController(private val dictionaryService: DictionaryService) {

    @GetMapping("contractors")
    fun getAllContractors(): List<ContractorDto> {
        return dictionaryService.getAllContractors();
    }

    @GetMapping("contractors/{contractorId}")
    fun getContractor(@PathVariable("contractorId") contractorId: Long): ContractorDto {
        return dictionaryService.getContractor(contractorId);
    }

    @GetMapping("products")
    fun getAllProducts(): List<ProductDto> {
        return dictionaryService.getAllProducts();
    }

    @GetMapping("products/{productId}")
    fun getProduct(@PathVariable("productId") productId: Long): ProductDto {
        return dictionaryService.getProduct(productId);
    }
}