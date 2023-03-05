package io.devs.asmachineservice.controllers

import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("root")
class RootController {

    @GetMapping("{text}")
    fun getTextString(@PathVariable("text") text: String): String {
        return text;
    }
}