export const TextArea = ({ className }: { className?: string }) => {
  return (
    <div>
      <textarea
        id="message"
        rows={3}
        className={`${className} focus:outline-none block p-2.5 w-full text-xs text-gray-900 rounded-lg border border-gray-300 dark:bg-[#202324] dark:text-white`}
      ></textarea>
    </div>
  );
};
