package net.fisenko.dictum.model.domain;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public final class Language {

    private Long id;
    private String code;
    private String language;
}
