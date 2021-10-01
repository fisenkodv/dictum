package net.fisenko.dictum.business.service;

import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.core.service.MappingService;
import org.modelmapper.ModelMapper;
import org.modelmapper.Module;
import org.modelmapper.convention.MatchingStrategies;

import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;

@Slf4j
public class MappingServiceImpl implements MappingService {

    private final ModelMapper modelMapper;

    public MappingServiceImpl(Module... modules) {
        this.modelMapper = new ModelMapper();
        this.modelMapper.getConfiguration().setAmbiguityIgnored(true);
        this.modelMapper.getConfiguration().setMatchingStrategy(MatchingStrategies.STRICT);

        try {
            for (Module module : modules) {
                this.modelMapper.registerModule(module);
            }
            this.modelMapper.validate();
        } catch (Exception e) {
            log.error("Unable to initialize mapping service", e);
        }
    }

    @Override
    public <TOut> TOut map(Object source, Class<TOut> destinationType) {
        try {
            return source != null
                    ? modelMapper.map(source, destinationType)
                    : null;
        } catch (Throwable e) {
            log.error("Unable to convert {} to {}", source, destinationType, e);
            return null;
        }
    }

    @Override
    public <TOut> Collection<TOut> map(Collection<?> source, Class<TOut> destinationType) {
        if (source == null) {
            return List.of();
        }

        return source.stream().map(x -> map(x, destinationType)).collect(Collectors.toList());
    }
}
