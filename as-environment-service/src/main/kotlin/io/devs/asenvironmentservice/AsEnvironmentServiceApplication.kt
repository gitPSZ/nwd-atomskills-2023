package io.devs.asenvironmentservice

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.transaction.annotation.EnableTransactionManagement

@SpringBootApplication
@EnableTransactionManagement
class AsEnvironmentServiceApplication

fun main(args: Array<String>) {
	runApplication<AsEnvironmentServiceApplication>(*args)
}
