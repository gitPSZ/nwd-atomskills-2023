package io.devs.asmachineservice.configurations

import org.apache.catalina.connector.Connector
import org.springframework.beans.factory.annotation.Value
import org.springframework.boot.autoconfigure.web.ServerProperties
import org.springframework.boot.autoconfigure.web.servlet.TomcatServletWebServerFactoryCustomizer
import org.springframework.boot.web.embedded.tomcat.TomcatServletWebServerFactory
import org.springframework.boot.web.server.WebServerFactoryCustomizer
import org.springframework.boot.web.servlet.FilterRegistrationBean
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration


@Configuration
class PortConfiguration(
    @Value("\${server.port:8080}") private var serverPort: Long,
    @Value("#{\${server.milling-ports}}") private var millingPorts: HashMap<String, Long>,
    @Value("#{\${server.lathe-ports}}") private var lathePorts: HashMap<String, Long>
) {

    @Bean
    fun trustedEndpointsFilter(): FilterRegistrationBean<MultiplePortFilter>? {
        return FilterRegistrationBean<MultiplePortFilter>(MultiplePortFilter(millingPorts, lathePorts));
    }

    @Bean
    fun servletContainer(): WebServerFactoryCustomizer<TomcatServletWebServerFactory> {
        val serverProperties = ServerProperties();
        val additionalConnectors: List<Connector> = additionalConnector()
        return MultyPortsWebServerFactoryCustomizer(serverProperties, additionalConnectors);
    }

    private fun additionalConnector(): List<Connector> {
        val additionalPortsMap: HashMap<String, Long> = HashMap();
        additionalPortsMap.putAll(this.millingPorts);
        additionalPortsMap.putAll(this.lathePorts);
        if (additionalPortsMap.isEmpty()) {
            return ArrayList();
        }
        return additionalPortsMap.entries
            .filter { entry -> entry.value != this.serverPort }
            .map { entry ->
                val connector = Connector("org.apache.coyote.http11.Http11NioProtocol");
                connector.scheme = "http"
                connector.port = entry.value.toInt();
                return@map connector;
            };
    }
}

private class MultyPortsWebServerFactoryCustomizer(
    serverProperties: ServerProperties,
    private val additionalConnectors: List<Connector>
) : TomcatServletWebServerFactoryCustomizer(serverProperties) {
    override fun customize(factory: TomcatServletWebServerFactory?) {
        super.customize(factory)
        if (additionalConnectors.isNotEmpty()) {
            factory?.addAdditionalTomcatConnectors(*additionalConnectors.toTypedArray());
        }
    }
}