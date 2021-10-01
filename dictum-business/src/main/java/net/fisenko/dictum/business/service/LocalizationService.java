package net.fisenko.dictum.business.service;

import java.text.MessageFormat;
import java.util.Locale;
import java.util.ResourceBundle;

public class LocalizationService {

    private final static Locale locale = Locale.getDefault();
    private final static ResourceBundle resourceBundle = ResourceBundle.getBundle("messages", locale);

    public static String getMessage(String label) {
        return resourceBundle.containsKey(label) ? resourceBundle.getString(label) : label;
    }

    public static String getMessage(String label, Object... arguments) {
        final String pattern = getMessage(label);
        final MessageFormat formatter = new MessageFormat(pattern, locale);

        final Object[] arrayOfArgs = new Object[arguments.length];
        System.arraycopy(arguments, 0, arrayOfArgs, 0, arguments.length);

        return formatter.format(arrayOfArgs);
    }
}
