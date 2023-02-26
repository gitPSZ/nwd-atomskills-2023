package io.devs.asenvironmentservice.controllers

import io.devs.asenvironmentservice.services.EnvironmentValueService
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("crm/requests")
class CrmRequestController(private val environmentValueService: EnvironmentValueService) {

    @GetMapping
    fun getAllRequests(): String {
        return "all requests ${environmentValueService.getMaxExecutingRequests()}";
    }

    @GetMapping("{id}")
    fun getRequest(@PathVariable("id") requestId: Long): String {
        return "request $requestId returned";
    }
}