package net.fisenko.dictum.core.model.domain;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
@AllArgsConstructor
public class Statistics {

    private long authors;
    private long quotes;
}
