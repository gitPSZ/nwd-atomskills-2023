package io.devs.asenvironmentservice.controllers

import com.tej.JooQDemo.jooq.sample.model.*
import com.tej.JooQDemo.jooq.sample.model.DefaultCatalog.*
import org.springframework.beans.factory.annotation.Value
import org.springframework.web.bind.annotation.*


@RestController
@CrossOrigin("*")
@RequestMapping("mnf")
class MnfController(
    @Value("#{\${server.milling-ports}}") private var millingPorts: HashMap<String, Long>,
    @Value("#{\${server.lathe-ports}}") private var lathePorts: HashMap<String, Long>
) {

    @GetMapping("machines")
    fun getHello(): HashMap<String, HashMap<String, Long>> {
        return hashMapOf(
            Pair("lathe", lathePorts),
            Pair("milling", millingPorts)
        );
    }
}