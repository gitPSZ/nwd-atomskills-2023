package io.devs.asenvironmentservice.services

import org.springframework.core.env.Environment
import org.springframework.stereotype.Service


@Service
class EnvironmentValueService(private val environment: Environment) {
    fun getMaxExecutingRequests(): Long {
        return this.environment.getProperty("crm.max-executing-requests")!!.toLong();
    }
}