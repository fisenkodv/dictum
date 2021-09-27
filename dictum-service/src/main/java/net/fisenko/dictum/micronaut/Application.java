package net.fisenko.dictum.micronaut;

import io.micronaut.runtime.Micronaut;
import reactor.tools.agent.ReactorDebugAgent;

public class Application {

    public static void main(String[] args) {
        ReactorDebugAgent.init();
        Micronaut.run(Application.class, args);
    }
}
