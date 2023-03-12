package io.devs.asenvironmentservice.services

import com.tej.JooQDemo.jooq.sample.model.dictionaries.Tables.CONTRACTOR
import com.tej.JooQDemo.jooq.sample.model.dictionaries.Tables.PRODUCTS
import io.devs.asenvironmentservice.dto.ContractorDto
import io.devs.asenvironmentservice.dto.ProductDto
import org.jooq.DSLContext
import org.springframework.stereotype.Service

@Service
class DictionaryService(private val dsl: DSLContext) {

    fun getAllContractors(): List<ContractorDto> {
        return dsl.select(
            CONTRACTOR.ID,
            CONTRACTOR.S_INN,
            CONTRACTOR.S_CAPTION
        )
            .from(CONTRACTOR)
            .fetch { record ->
                ContractorDto(
                    id = record.get(CONTRACTOR.ID),
                    inn = record.get(CONTRACTOR.S_INN),
                    caption = record.get(CONTRACTOR.S_CAPTION)
                )
            };
    }

    fun getContractor(contractorId: Long): ContractorDto {
        return dsl.select(
            CONTRACTOR.ID,
            CONTRACTOR.S_INN,
            CONTRACTOR.S_CAPTION
        )
            .from(CONTRACTOR)
            .where(CONTRACTOR.ID.eq(contractorId))
            .fetchOne { record ->
                ContractorDto(
                    id = record.get(CONTRACTOR.ID),
                    inn = record.get(CONTRACTOR.S_INN),
                    caption = record.get(CONTRACTOR.S_CAPTION)
                )
            } ?: ContractorDto();
    }

    fun getAllProducts(): List<ProductDto> {
        return dsl.select(
            PRODUCTS.ID,
            PRODUCTS.S_CODE,
            PRODUCTS.S_CAPTION,
            PRODUCTS.N_LATHE_TIME,
            PRODUCTS.N_MILLING_TIME
        )
            .from(PRODUCTS)
            .fetch { record ->
                ProductDto(
                    id = record.get(PRODUCTS.ID),
                    code = record.get(PRODUCTS.S_CODE),
                    caption = record.get(PRODUCTS.S_CAPTION),
                    latheTime = record.get(PRODUCTS.N_LATHE_TIME).toLong(),
                    millingTime = record.get(PRODUCTS.N_MILLING_TIME).toLong()
                )
            };
    }

    fun getProduct(productId: Long): ProductDto {
        return dsl.select(
            PRODUCTS.ID,
            PRODUCTS.S_CODE,
            PRODUCTS.S_CAPTION,
            PRODUCTS.N_LATHE_TIME,
            PRODUCTS.N_MILLING_TIME
        )
            .from(PRODUCTS)
            .where(PRODUCTS.ID.eq(productId))
            .fetchOne { record ->
                ProductDto(
                    id = record.get(PRODUCTS.ID),
                    code = record.get(PRODUCTS.S_CODE),
                    caption = record.get(PRODUCTS.S_CAPTION),
                    latheTime = record.get(PRODUCTS.N_LATHE_TIME).toLong(),
                    millingTime = record.get(PRODUCTS.N_MILLING_TIME).toLong()
                )
            } ?: ProductDto();
    }
}