package net.fisenko.dictum.model.domain;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class Quote {

    private Long id;
    private String text;
    private Author author;
    private Language language;
}
