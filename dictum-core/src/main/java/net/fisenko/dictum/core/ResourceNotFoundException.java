package net.fisenko.dictum.core;

import net.fisenko.dictum.core.business.service.LocalizationService;

public class ResourceNotFoundException  extends RuntimeException {

    public ResourceNotFoundException(String label) {
        super(LocalizationService.getMessage(label));
    }

    public ResourceNotFoundException(String label, Object... arguments) {
        super(LocalizationService.getMessage(label, arguments));
    }
}
