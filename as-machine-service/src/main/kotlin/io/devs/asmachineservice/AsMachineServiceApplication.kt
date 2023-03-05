package io.devs.asmachineservice

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication

@SpringBootApplication
class AsMachineServiceApplication

fun main(args: Array<String>) {
	runApplication<AsMachineServiceApplication>(*args)
}
