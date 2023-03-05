package io.devs.asmachineservice.configurations

import javax.servlet.FilterChain
import javax.servlet.ServletRequest
import javax.servlet.ServletResponse
import javax.servlet.http.HttpServletRequest
import javax.servlet.http.HttpServletRequestWrapper
import javax.servlet.http.HttpServletResponse


class MultiplePortFilter(
    private val millingPorts: HashMap<String, Long>,
    private val lathePorts: HashMap<String, Long>
) : javax.servlet.Filter {
    override fun doFilter(
        servletRequest: ServletRequest,
        servletResponse: ServletResponse,
        filterChain: FilterChain
    ) {
        var request = servletRequest as HttpServletRequest;
        val response = servletResponse as HttpServletResponse;
        request = redirectConfig(request, millingPorts, lathePorts);
        filterChain.doFilter(request, response);
    }

    private fun redirectConfig(
        request: HttpServletRequest,
        millingPorts: HashMap<String, Long>,
        lathePorts: HashMap<String, Long>
    ): HttpServletRequest {
        val path: String = request.requestURI;
        val port: Int = request.localPort;
        var requestType = "root";
        var machineModel = "";
        if (millingPorts.values.any { millingPort -> millingPort == port.toLong() }) {
            requestType = "milling";
            machineModel = millingPorts.entries
                .filter { entry -> entry.value == port.toLong() }
                .map { entry -> entry.key }[0];
        }
        if (lathePorts.values.any { lathePort -> lathePort == port.toLong() }) {
            requestType = "lathe";
            machineModel = lathePorts.entries
                .filter { entry -> entry.value == port.toLong() }
                .map { entry -> entry.key }[0];
        }
        if (requestType != "root") {
            val toUri: String = "/$requestType/$machineModel$path";
            return object : HttpServletRequestWrapper(request) {
                override fun getRequestURI(): String {
                    return toUri;
                }
            }
        }
        return request;
    }
}
