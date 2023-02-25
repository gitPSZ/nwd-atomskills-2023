package io.devs.asenvironmentservice.controllers

import com.tej.JooQDemo.jooq.sample.model.*
import com.tej.JooQDemo.jooq.sample.model.DefaultCatalog.*
import org.jooq.DSLContext
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.web.bind.annotation.CrossOrigin
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RestController


@RestController
@CrossOrigin("*")
class RootController {

    @Autowired
    lateinit var dsl: DSLContext;

    @GetMapping("hello/{text}")
    fun getHello(@PathVariable("text") message: String): String {
        return "answer $message";
    }
}