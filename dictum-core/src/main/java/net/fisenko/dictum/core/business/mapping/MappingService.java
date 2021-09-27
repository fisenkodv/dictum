package net.fisenko.dictum.core.business.mapping;

import java.util.Collection;

public interface MappingService {

    <TOut> TOut map(Object source, Class<TOut> destinationType);

    <TOut> Collection<TOut> map(Collection<?> source, Class<TOut> destinationType);
}