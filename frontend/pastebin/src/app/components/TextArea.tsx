export const TextArea = ({
  className,
  value,
  readonly,
  name
}: {
  className?: string;
  value?: string;
  readonly?: boolean;
  name?: string;
}) => {
  return (
    <div>
      <textarea
        id="message"
        rows={3}
        className={`${className} focus:outline-none block p-2.5 w-full text-xs text-gray-900 rounded-lg border border-gray-300 dark:bg-[#202324] dark:text-white`}
        value={value}
        autoCorrect="off"
        spellCheck="false" 
        autoCapitalize="none"
        readOnly={readonly}
        name={name}
      ></textarea>
    </div>
  );
};
