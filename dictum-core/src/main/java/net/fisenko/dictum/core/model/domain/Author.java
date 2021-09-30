package net.fisenko.dictum.core.model.domain;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class Author {

    private String id;
    private String name;
    private String bio;
    private String language;
}
