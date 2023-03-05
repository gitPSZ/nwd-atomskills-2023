package io.devs.asmachineservice.controllers

import org.springframework.stereotype.Controller
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("lathe")
class LatheController {
    @GetMapping("{model}/who-am-i")
    fun getWhoAmI(@PathVariable("model") machineModel: String): String {
        return "I am a $machineModel lathe controller";
    }
}