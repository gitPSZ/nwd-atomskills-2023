package io.devs.asenvironmentservice.controllers

import io.devs.asenvironmentservice.dto.MachineLogDto
import io.devs.asenvironmentservice.enums.MachineStates
import io.devs.asenvironmentservice.services.MachineService
import org.jooq.JSONB
import org.springframework.web.bind.annotation.*

@RestController
@CrossOrigin("*")
@RequestMapping("milling")
class MillingController(private val machineService: MachineService) {

    private val MACHINE_TYPE: String = "milling";

    @GetMapping("{machineCode}/status/all")
    fun getAllStatuses(@PathVariable("machineCode") machineCode: String): List<MachineLogDto> {
        return machineService.getAllStatuses(machineCode);
    }

    @GetMapping("{machineCode}/status")
    fun getStatus(@PathVariable("machineCode") machineCode: String): MachineLogDto {
        return machineService.getStatus(machineCode);
    }

    @PostMapping("{machineCode}/set/waiting")
    fun setWaiting(
        @PathVariable("machineCode") machineCode: String,
        @RequestBody advInfo: String
    ) {
        machineService.setMachineStatus(machineCode, MachineStates.WAITING, JSONB.valueOf(advInfo), MACHINE_TYPE);
    }

    @PostMapping("{machineCode}/set/working")
    fun setWorking(
        @PathVariable("machineCode") machineCode: String,
        @RequestBody advInfo: String
    ) {
        machineService.setMachineStatus(machineCode, MachineStates.WORKING, JSONB.valueOf(advInfo), MACHINE_TYPE);
    }

    @PostMapping("{machineCode}/set/broken")
    fun setBroken(
        @PathVariable("machineCode") machineCode: String,
        @RequestBody advInfo: String
    ) {
        machineService.setMachineStatus(machineCode, MachineStates.BROKEN, JSONB.valueOf(advInfo), MACHINE_TYPE);
    }

    @PostMapping("{machineCode}/set/repairing")
    fun setRepairing(
        @PathVariable("machineCode") machineCode: String,
        @RequestBody advInfo: String
    ) {
        machineService.setMachineStatus(machineCode, MachineStates.REPAIRING, JSONB.valueOf(advInfo), MACHINE_TYPE);
    }
}