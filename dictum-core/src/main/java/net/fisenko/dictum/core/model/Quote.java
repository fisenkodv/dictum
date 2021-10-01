package net.fisenko.dictum.core.model;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class Quote {

    private String id;
    private String text;
    private Author author;
    private String language;
}
