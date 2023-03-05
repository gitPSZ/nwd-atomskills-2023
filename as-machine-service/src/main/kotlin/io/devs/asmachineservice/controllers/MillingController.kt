package io.devs.asmachineservice.controllers

import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("milling")
class MillingController {
    @GetMapping("{model}/who-am-i")
    fun getWhoAmI(@PathVariable("model") machineModel: String): String {
        return "I am a $machineModel milling controller";
    }
}