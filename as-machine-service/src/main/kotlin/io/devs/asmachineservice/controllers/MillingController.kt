package io.devs.asmachineservice.controllers

import io.devs.asmachineservice.dto.MachineLogDto
import io.devs.asmachineservice.enums.MachineStates
import io.devs.asmachineservice.services.MachineService
import org.jooq.JSONB
import org.springframework.web.bind.annotation.*

@RestController
@RequestMapping("milling")
class MillingController(private val machineService: MachineService) {
    @GetMapping("{machineCode}/status")
    fun getStatus(@PathVariable("machineCode") machineCode: String): MachineLogDto {
        return machineService.getStatus(machineCode);
    }

    @PostMapping("{machineCode}/set/waiting")
    fun setWaiting(
        @PathVariable("machineCode") machineCode: String,
        @RequestBody advInfo: String
    ) {
        machineService.setMachineStatus(machineCode, MachineStates.WAITING, JSONB.valueOf(advInfo))
    }

    @PostMapping("{machineCode}/set/working")
    fun setWorking(
        @PathVariable("machineCode") machineCode: String,
        @RequestBody advInfo: String
    ) {
        machineService.setMachineStatus(machineCode, MachineStates.WORKING, JSONB.valueOf(advInfo))
    }

    @PostMapping("{machineCode}/set/broken")
    fun setBroken(
        @PathVariable("machineCode") machineCode: String,
        @RequestBody advInfo: String
    ) {
        machineService.setMachineStatus(machineCode, MachineStates.BROKEN, JSONB.valueOf(advInfo))
    }
}